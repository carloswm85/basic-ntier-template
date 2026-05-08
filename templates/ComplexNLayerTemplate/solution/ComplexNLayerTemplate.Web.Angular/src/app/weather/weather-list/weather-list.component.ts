
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-weather-list',
  imports: [],
  templateUrl: './weather-list.component.html',
  styleUrl: './weather-list.component.css'
})
export class WeatherListComponent implements OnInit {
    public forecasts: WeatherForecast[] = [];

    constructor(private readonly http: HttpClient) { }

    ngOnInit() {
        this.getForecasts();
    }

    getForecasts() {
        this.http.get<WeatherForecast[]>('api/v1/WeatherForecasts/weatherList').subscribe(

            (result) => {
                this.forecasts = result;
                console.log(">>> RESULT!");
                console.log(result);
            },
            (error) => {
                console.error(error);
            }
        );
    }

}

interface WeatherForecast {
    Date: string;
    TemperatureC: number;
    TemperatureF: number;
    Summary: string;
}
