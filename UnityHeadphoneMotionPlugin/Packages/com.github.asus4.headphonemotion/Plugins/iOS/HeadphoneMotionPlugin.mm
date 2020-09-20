//
//  HeadphoneMotionPlugin.m
//  HeadphoneMotionPlugin
//
//  Created by Koki Ibukuro on 2020/09/16.
//

#import <Foundation/Foundation.h>
#import <CoreMotion/CoreMotion.h>


#pragma mark - typedef

typedef struct {
    float x;
    float y;
    float z;
} float3;

typedef struct {
    float x;
    float y;
    float z;
    float w;
} float4;

typedef struct {
    float3 userAcceleration;
    float4 rotation;
    CMDeviceMotionSensorLocation location;
} HeadphoneMotionData;

typedef void (*UnityHeadphoneMotionCallback)(HeadphoneMotionData motion);
typedef void (*UnityHeadphoneMotionEventCallback)(BOOL connected);


#pragma mark - Headphone Motion Plugin


@interface HeadphoneMotionPlugin : NSObject <CMHeadphoneMotionManagerDelegate>
{
    CMHeadphoneMotionManager* motionManager;
}

@property (nonatomic) UnityHeadphoneMotionEventCallback eventCallback;

@end

@implementation HeadphoneMotionPlugin

// Singletone
static HeadphoneMotionPlugin * _shared;

+ (HeadphoneMotionPlugin*) shared {
    @synchronized(self) {
        if(_shared == nil) {
            _shared = [[self alloc] init];
        }
    }
    return _shared;
}


- (id) init {
    if (self = [super init])
    {
        motionManager = [CMHeadphoneMotionManager new];
        motionManager.delegate = self;
    }
    return self;
}

- (BOOL) isAvailable {
    return motionManager.isDeviceMotionAvailable;
}

- (BOOL) isActive {
    return motionManager.isDeviceMotionActive;
}

- (void) start:(UnityHeadphoneMotionCallback) callback {
    [motionManager startDeviceMotionUpdatesToQueue:NSOperationQueue.mainQueue
                                       withHandler:^(CMDeviceMotion * _Nullable motion, NSError * _Nullable error) {
        if(error != NULL) {
            NSLog(@"headphone motion error: %@", error);
            return;
        }
        if(motion == NULL) {
            return;
        }

        HeadphoneMotionData data;
        data.location = motion.sensorLocation;
        
        CMAcceleration acc = motion.userAcceleration;
        data.userAcceleration = float3 {
            (float)acc.x,
            (float)acc.y,
            (float)acc.z
        };
        
        // Convert quaternion to Unity space
        CMQuaternion rot = motion.attitude.quaternion;
        data.rotation = float4 {
            (float)rot.x,
            (float)rot.z,
            (float)rot.y,
            (float)-rot.w
        };

        callback(data);
    }];
}

- (void) stop {
    [motionManager stopDeviceMotionUpdates];
}

- (void)headphoneMotionManagerDidConnect:(CMHeadphoneMotionManager *)manager {
    if(self.eventCallback != NULL) {
        self.eventCallback(YES);
    }
}

- (void)headphoneMotionManagerDidDisconnect:(CMHeadphoneMotionManager *)manager {
    if(self.eventCallback != NULL) {
        self.eventCallback(NO);
    }
}

@end

#pragma mark - Unity Bridge

extern "C" {
    bool _unityHeadphoneDeviceMotionIsAvailable(void) {
        return HeadphoneMotionPlugin.shared.isAvailable;
    }

    bool _unityHeadphoneDeviceMotionIsActive(void) {
        return HeadphoneMotionPlugin.shared.isActive;
    }

    void _unityHeadphoneMotionStart(UnityHeadphoneMotionCallback callback) {
        [HeadphoneMotionPlugin.shared start: callback];
    }

    void _unityHeadphoneMotionStop(void) {
        [HeadphoneMotionPlugin.shared stop];
    }

    void _unityHeadphoneMotionSetEventCallback(UnityHeadphoneMotionEventCallback callback) {
        HeadphoneMotionPlugin.shared.eventCallback = callback;
    }
}
