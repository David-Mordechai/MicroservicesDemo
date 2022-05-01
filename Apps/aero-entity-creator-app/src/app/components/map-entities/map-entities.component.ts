import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroupDirective, Validators } from '@angular/forms';
import { fromEvent, Observable, of } from 'rxjs';
import { MapEntityService } from 'src/app/services/map-entity.service';
import { MapsService } from 'src/app/services/maps.service';

@Component({
  selector: 'app-map-entities',
  templateUrl: './map-entities.component.html',
  styleUrls: ['./map-entities.component.css']
})
export class MapEntitiesComponent implements OnInit {

  @ViewChild('imageDiv') imageDiv: ElementRef<HTMLDivElement> | undefined;

  missionMap: string = '';

  get title() { return this.mapEntityForm.get('title'); }
  get xPosition() { return this.mapEntityForm.get('xPosition'); }
  get yPosition() { return this.mapEntityForm.get('yPosition'); }

  mapEntityForm = this.formBuilder.group({
    title: new FormControl('', [Validators.required]),
    xPosition: new FormControl(null, [Validators.required]),
    yPosition: new FormControl(null, [Validators.required]),
    mapWidth: new FormControl(Number),
    mapHeight: new FormControl(Number)
  });

  constructor(private mapEntityService: MapEntityService,
    private formBuilder: FormBuilder, private mapsService: MapsService) { }

  ngOnInit(): void {
    this.loadMissionMap();
  }

  ngAfterViewInit() {
    fromEvent(this.imageDiv?.nativeElement!, 'click').subscribe(event => {
      let pointerEvent = event as PointerEvent;
      let imageDiv = event.target as Element;

      this.mapEntityForm.controls['yPosition'].setValue(pointerEvent.offsetX);
      this.mapEntityForm.controls['xPosition'].setValue(pointerEvent.offsetY);
      this.mapEntityForm.controls['mapWidth'].setValue(imageDiv.clientWidth);
      this.mapEntityForm.controls['mapHeight'].setValue(imageDiv.clientHeight);
    });
  }

  onSubmit(formDirective: FormGroupDirective): void {
    this.mapEntityService.postMapEntity(this.mapEntityForm.value);
    this.mapEntityForm.reset();
    formDirective.resetForm();
  }

  loadMissionMap() {
    this.mapsService.getMissionMap().subscribe({
      next: (response) => {
        if (response && response.success) {
          this.missionMap = `${response.mapFileAsBase64String.imageMetaData},${response.mapFileAsBase64String.imageBase64}`;
        }
        else {
          this.missionMap = '';
        }
      },
    });
  }
}


