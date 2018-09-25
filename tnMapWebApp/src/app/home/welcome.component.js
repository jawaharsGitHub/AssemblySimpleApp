"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require("@angular/core");
var platform_browser_1 = require("@angular/platform-browser");
var WelcomeComponent = (function () {
    function WelcomeComponent(domSanitizer, renderer) {
        this.domSanitizer = domSanitizer;
        this.renderer = renderer;
        this.name = "Hello";
        this.srcValue = './assets/files/tnDist1.svg';
    }
    WelcomeComponent.prototype.callRect = function () {
        alert("Rect clicked");
    };
    WelcomeComponent.prototype.ngOnInit = function () {
        var _this = this;
        var URL = './assets/files/tamilnaduMap.svg';
        this.srcValue = this.domSanitizer.bypassSecurityTrustResourceUrl(URL);
        var rect = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
        this.renderer.listen(rect, 'click', function (evt) {
            _this.callRect();
            console.log('Clicking the rect', evt);
        });
    };
    WelcomeComponent.prototype.ShowDistrict = function () {
        var URL = './assets/files/dist.svg';
        this.srcValue = this.domSanitizer.bypassSecurityTrustResourceUrl(URL);
        //alert(this.srcValue);
    };
    WelcomeComponent.prototype.ShowState = function () {
        var URL = './assets/files/tamilnaduMap.svg';
        this.srcValue = this.domSanitizer.bypassSecurityTrustResourceUrl(URL);
    };
    return WelcomeComponent;
}());
WelcomeComponent = __decorate([
    core_1.Component({
        templateUrl: './app/home/welcome.component.html'
    }),
    __metadata("design:paramtypes", [platform_browser_1.DomSanitizer, core_1.Renderer2])
], WelcomeComponent);
exports.WelcomeComponent = WelcomeComponent;
//# sourceMappingURL=welcome.component.js.map