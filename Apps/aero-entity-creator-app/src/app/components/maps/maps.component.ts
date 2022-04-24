import { Component, OnInit } from '@angular/core';
import { Observable, of, Subject } from 'rxjs';
import { MapsService } from 'src/app/services/maps.service';

@Component({
  selector: 'app-maps',
  templateUrl: './maps.component.html',
  styleUrls: ['./maps.component.css']
})
export class MapsComponent implements OnInit {

  maps!: Observable<string[]>;
  constructor(private mapsService: MapsService) { }
  
  ngOnInit(): void {
    this.loadData();
  }

  private loadData() {
    this.maps = this.mapsService.getMaps();
  }

  loadMap(map: string){
    console.log(map);
  }
}
