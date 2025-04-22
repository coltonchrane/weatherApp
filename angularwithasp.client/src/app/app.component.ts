import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

interface WeatherForecast {
  time: string;
  lowTemp: number;
  highTemp: number;
}

interface Coordinates {
  latitude: string,
  longitude: string
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  public coord: Coordinates =
    {
      latitude: "",
      longitude: ""
    };
  public forecasts!: WeatherForecast[];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    const options = {
      enableHighAccuracy: true,
      timeout: 5000,
      maximumAge: 0,
    };

    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition((position) => {
        this.coord.latitude = position.coords.latitude.toString();
        this.coord.longitude = position.coords.longitude.toString();

        this.getForecasts(this.coord.latitude, this.coord.longitude);
      },
        this.error,
        options);
    }
  }

  getForecasts(lat: string, long: string): void {
    this.http.get<WeatherForecast[]>('/weatherforecast?lon='+long+'&lat='+lat).subscribe(
      (result: any) => {
        this.forecasts = result;
      },
      (error) => {
        console.error(error);
      }
    )
  }

  title = 'weatherApp.client';

  error(error:any) {
    console.warn(`ERROR(${error.code}): ${error.message}`);
  };

}

