import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

interface WeatherForecast {
  time: string;
  lowTemp: number;
  highTemp: number;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  public forecasts: WeatherForecast[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    const options = {
      enableHighAccuracy: true,
      timeout: 5000,
      maximumAge: 0,
    };

    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition((position) => {
        let lat = position.coords.latitude.toString();
        let long = position.coords.longitude.toString();
        console.log('Your latitude is :' + lat);
        console.log('Your longitude is :' + long);

        this.getForecasts(lat, long);
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

