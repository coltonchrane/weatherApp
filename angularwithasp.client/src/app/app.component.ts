import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

interface WeatherForecast {
  time: string;
  lowTemp: number;
  highTemp: number;
}

interface WeatherPage {
  location: string,
  forecasts: WeatherForecast[]
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  public weather: WeatherPage =
    {
      location: "",
      forecasts: []
    };
  public isLoading = true;

  constructor(private http: HttpClient) {}

  ngOnInit() {
    const options = {
      enableHighAccuracy: true,
      timeout: 10000,
      maximumAge: 0,
    };

    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition((position) => {
        let latitude = position.coords.latitude.toString();
        let longitude = position.coords.longitude.toString();

        this.getForecasts(latitude, longitude);
      },
        (err) => {
          this.error(err);
          this.isLoading = false;
        },
        options);
    } else {
      this.isLoading = false;
    }
  }

  getForecasts(lat: string, long: string): void {
    this.http.get<WeatherForecast[]>('/weatherforecast?lon='+long+'&lat='+lat).subscribe(
      (result: any) => {
        this.weather = result;
        this.isLoading = false;
      },
      (error: any) => {
        console.error(error);
        this.isLoading = false;
      }
    )
  }

  title = 'weatherApp.client';

  error(error:any) {
    console.warn(`ERROR(${error.code}): ${error.message}`);
  };

}

