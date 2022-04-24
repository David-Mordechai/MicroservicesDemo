import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MapEntityService } from 'src/app/services/map-entity.service';

@Component({
  selector: 'app-map-entities',
  templateUrl: './map-entities.component.html',
  styleUrls: ['./map-entities.component.css']
})
export class MapEntitiesComponent implements OnInit {

  constructor(private mapEntityService: MapEntityService, private formBuilder: FormBuilder) { }

  mapEntityForm = this.formBuilder.group({
    title: '',
    xPosition: Number,
    yPosition: Number
  });

  ngOnInit(): void {

  }

  onSubmit(): void {
    this.mapEntityService.postMapEntity(this.mapEntityForm.value);
    console.log('New map entity was submitted', this.mapEntityForm.value);
    this.mapEntityForm.reset();
  }
}
