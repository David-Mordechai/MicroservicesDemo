import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable, of } from 'rxjs';
import { mapListItem } from 'src/app/models/mapListItem';
import { MapsService } from 'src/app/services/maps.service';
import { FileUploadComponent } from '../file-upload/file-upload.component';

@Component({
  selector: 'app-maps',
  templateUrl: './maps.component.html',
  styleUrls: ['./maps.component.css']
})
export class MapsComponent implements OnInit {

  maps!: Observable<mapListItem[]>;
  mapBase64!: Observable<string>;
  selectedMap: mapListItem | undefined;
  constructor(private mapsService: MapsService, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.loadData();
  }

  private loadData() {
    this.maps = this.mapsService.getMaps();
    this.maps.subscribe(maps => {
      let missionMap = maps.find(map=> map.isMissionMap);
      if(missionMap){
        this.loadMap(missionMap);
      }
    });
  }

  loadMap(selectedMap: mapListItem){
      this.mapsService.getMap(selectedMap.mapName).subscribe({next: (response) => {
        if(response && response.success){
          this.mapBase64 = of(`${response.mapFileAsBase64String.imageMetaData},${response.mapFileAsBase64String.imageBase64}`);
          this.selectedMap = selectedMap;
        }
        else{
          this.mapBase64 = of('');
          this.selectedMap = undefined;
        }
      },
    });
  }

  deleteMap(){
    this.mapsService.deleteMap(this.selectedMap!.mapName).subscribe(()=>{
      this.loadData();
      this.mapBase64 = of('');
      this.selectedMap = undefined;
    });
  }

  setMissionMap(){
    this.mapsService.setMissionMap(this.selectedMap!.mapName).subscribe(() =>{
      this.loadData();
      this.selectedMap!.isMissionMap = true;
    });
  }

  openDialog() {
    const dialogRef = this.dialog.open(FileUploadComponent,{
      height: '320px',
      width: '300px'
    });

    dialogRef.componentInstance.notify.subscribe(() => {
      this.loadData();
    });
  }
}
