//
//  ViewController.m
//  HeadphoneMotionPlugin
//
//  Created by Koki Ibukuro on 2020/09/16.
//

#import "ViewController.h"
#import <CoreMotion/CoreMotion.h>

typedef struct {
    CMAcceleration userAcceleration;
    CMQuaternion rotation;
    CMDeviceMotionSensorLocation location;
} HeadphoneMotionData;
typedef void (*UnityHeadphoneMotionCallback)(HeadphoneMotionData motion);
typedef void (*UnityHeadphoneMotionEventCallback)(BOOL connected);

extern bool _unityHeadphoneDeviceMotionIsAvailable(void);
extern bool _unityHeadphoneDeviceMotionIsActive(void);
extern void _unityHeadphoneMotionStart(UnityHeadphoneMotionCallback callback);
extern void _unityHeadphoneMotionStop(void);
extern void _unityHeadphoneMotionSetEventCallback(UnityHeadphoneMotionEventCallback callback);

static BOOL isConnected;
void onHeadphoneMotionEvent(BOOL connected){
    NSLog(@"OnHeadphoneMotionEvent: %@", connected ? @"Connected" : @"Disconnected");
    isConnected = connected;
}

static HeadphoneMotionData motionData;
void onHeadphoneMotion(HeadphoneMotionData data) {
    motionData = data;
}


@interface ViewController ()
{
}
@property (weak, nonatomic) IBOutlet UILabel *infoLabel;
@end

@implementation ViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    
    _unityHeadphoneMotionSetEventCallback(onHeadphoneMotionEvent);
    [self updateInfoLabel];
}

- (IBAction)onToggleStartButton:(id)sender {
    NSLog(@"onToggleStartButton");
    
    if(isConnected) {
        _unityHeadphoneMotionStop();
    } else {
        _unityHeadphoneMotionStart(onHeadphoneMotion);
    }
}

- (IBAction)onUpdateButton:(id)sender {
    [self updateInfoLabel];
}

- (NSString*) motionDataToString: (HeadphoneMotionData) data {
    
    NSString *location;
    switch (motionData.location) {
        case CMDeviceMotionSensorLocationDefault:
            location = @"default";
            break;
        case CMDeviceMotionSensorLocationHeadphoneLeft:
            location = @"left";
            break;
        case CMDeviceMotionSensorLocationHeadphoneRight:
            location = @"right";
            break;
        default:
            location = @"unknown";
            break;
    }
    CMAcceleration acc = motionData.userAcceleration;
    CMQuaternion rot = motionData.rotation;
    
    return [NSString stringWithFormat:
            @"loc: %@\nacc: (%.2f, %.2f, %.2f)\nrot: (%.2f, %.2f, %.2f, %.2f)",
            location,
            acc.x, acc.y, acc.z,
            rot.w, rot.x, rot.y, rot.z];
}

- (void)updateInfoLabel {
    
    
    self.infoLabel.text = [NSString stringWithFormat:
                           @"available: %i\nactive: %i\nconnected: %@\nmotion: %@",
                           _unityHeadphoneDeviceMotionIsAvailable(),
                           _unityHeadphoneDeviceMotionIsAvailable(),
                           isConnected ?  @"Connected" : @"Disconnected",
                           [self motionDataToString: motionData]];
}

@end
