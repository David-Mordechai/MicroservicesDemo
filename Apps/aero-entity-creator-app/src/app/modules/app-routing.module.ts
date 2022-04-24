import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MapEntitiesComponent } from '../components/map-entities/map-entities.component';
import { MapsComponent } from '../components/maps/maps.component';

const routes: Routes = [
  { path: '', redirectTo: "/entities", pathMatch:'full' },
  { path: 'entities', component: MapEntitiesComponent },
  { path: 'maps', component: MapsComponent },
    
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
