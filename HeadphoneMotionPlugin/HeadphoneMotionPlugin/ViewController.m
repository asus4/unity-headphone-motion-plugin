//
//  ViewController.m
//  HeadphoneMotionPlugin
//
//  Created by Koki Ibukuro on 2020/09/16.
//

#import "ViewController.h"


extern void _unityStopHeadphoneMotion(void);

@interface ViewController ()

@end

@implementation ViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    _unityStopHeadphoneMotion();
}


@end
