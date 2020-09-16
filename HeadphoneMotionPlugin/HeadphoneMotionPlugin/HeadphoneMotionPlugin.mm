//
//  HeadphoneMotionPlugin.m
//  HeadphoneMotionPlugin
//
//  Created by Koki Ibukuro on 2020/09/16.
//

#import <Foundation/Foundation.h>

#pragma mark - Unity Bridge

@interface HeadphoneMotionPlugin : NSObject
{
}
@end

@implementation HeadphoneMotionPlugin

- (void)viewDidLoad {
}

@end


extern "C" {
    /*
     motion QuaternionX 0.187218 QuaternionY -0.097452 QuaternionZ 0.050883 QuaternionW 0.976147 UserAccelX -0.000190 UserAccelY 0.021194 UserAccelZ -0.028151 RotationRateX 0.933931 RotationRateY 0.392370 RotationRateZ 0.340124 MagneticFieldX 0.000000 MagneticFieldY 0.000000 MagneticFieldZ 0.000000 MagneticFieldAccuracy -1 Heading 0.000000 SensorLocation 2 @ 4277.461094
     */

    typedef void (*UnityHeadphoneCallback)(int status);
        
    void _unityStopHeadphoneMotion() {
        NSLog(@"stop");
    }
}
