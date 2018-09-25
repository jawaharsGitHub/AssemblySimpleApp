import { Component, OnInit, Renderer2 } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { SafeResourceUrl } from '@angular/platform-browser/src/security/dom_sanitization_service';


@Component({
    templateUrl: './app/home/welcome.component.html'
})
export class WelcomeComponent  implements OnInit {
    
  name: string = "Hello";
  srcValue: SafeResourceUrl = './assets/files/tnDist1.svg';

  constructor(private domSanitizer: DomSanitizer, public renderer: Renderer2) {

    

    

   }

   callRect(){
    alert("Rect clicked");
    }


  ngOnInit() {
    let URL = './assets/files/tamilnaduMap.svg';
    this.srcValue = this.domSanitizer.bypassSecurityTrustResourceUrl(URL);

    const rect = document.createElementNS( 'http://www.w3.org/2000/svg' , 'rect' );
    this.renderer.listen(rect, 'click', (evt) => {
      this.callRect();
      console.log('Clicking the rect', evt);
    });

    

    
  }


  ShowDistrict()
  {
    let URL = './assets/files/dist.svg';
    this.srcValue = this.domSanitizer.bypassSecurityTrustResourceUrl(URL);
    //alert(this.srcValue);
  }

  ShowState()
  {
    let URL = './assets/files/tamilnaduMap.svg';
    this.srcValue = this.domSanitizer.bypassSecurityTrustResourceUrl(URL);
  }
}
