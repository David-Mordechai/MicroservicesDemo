import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'aero-entity-creator-app';

  navLinks: any[];
  activeLinkIndex = -1; 

  constructor(private route: ActivatedRoute, public router: Router){
    this.navLinks = [
      { label: 'Map Entites', link: './entities', index: 0 }, 
      { label: 'Maps', link: './maps', index: 1 }, 
    ];
  }

  ngOnInit(): void {
    this.router.events.subscribe((res) => {
        this.activeLinkIndex = this.navLinks.indexOf(this.navLinks.find(tab => tab.link === '.' + this.router.url));
    });
  }
}
