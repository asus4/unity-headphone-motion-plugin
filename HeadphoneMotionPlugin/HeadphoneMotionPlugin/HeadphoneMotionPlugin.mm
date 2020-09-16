//
//  HeadphoneMotionPlugin.m
//  HeadphoneMotionPlugin
//
//  Created by Koki Ibukuro on 2020/09/16.
//

#import <Foundation/Foundation.h>
#import <CoreMotion/CoreMotion.h>


#pragma mark - typedef


/*
 motion QuaternionX 0.187218 QuaternionY -0.097452 QuaternionZ 0.050883 QuaternionW 0.976147 UserAccelX -0.000190 UserAccelY 0.021194 UserAccelZ -0.028151 RotationRateX 0.933931 RotationRateY 0.392370 RotationRateZ 0.340124 MagneticFieldX 0.000000 MagneticFieldY 0.000000 MagneticFieldZ 0.000000 MagneticFieldAccuracy -1 Heading 0.000000 SensorLocation 2 @ 4277.461094
 */
typedef struct {
    CMAcceleration userAcceleration;
    CMQuaternion rotation;
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
        data.userAcceleration = motion.userAcceleration;
        data.rotation = motion.attitude.quaternion;
        data.location = motion.sensorLocation;

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
    bool _unityHeadphoneDeviceMotionIsAvailable() {
        return HeadphoneMotionPlugin.shared.isAvailable;
    }

    bool _unityHeadphoneDeviceMotionIsActive() {
        return HeadphoneMotionPlugin.shared.isActive;
    }

    void _unityHeadphoneMotionStart(UnityHeadphoneMotionCallback callback) {
        [HeadphoneMotionPlugin.shared start: callback];
    }

    void _unityHeadphoneMotionStop() {
        [HeadphoneMotionPlugin.shared stop];
    }

    void _unityHeadphoneMotionSetEventCallback(UnityHeadphoneMotionEventCallback callback) {
        HeadphoneMotionPlugin.shared.eventCallback = callback;
    }
}
