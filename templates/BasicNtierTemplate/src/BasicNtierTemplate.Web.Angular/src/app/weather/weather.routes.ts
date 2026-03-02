import { Routes } from '@angular/router';
import { WeatherListComponent } from './weather-list/weather-list.component';

export const WEATHER_ROUTES: Routes = [
    { path: '', component: WeatherListComponent },
];
