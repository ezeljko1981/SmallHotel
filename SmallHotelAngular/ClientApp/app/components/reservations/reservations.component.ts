import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
@Component({
    selector: 'reservations',
    templateUrl: './reservations.component.html'
})
export class ReservationsComponent {
    public reservations: ReservationExtended[];
    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        http.get(baseUrl + 'api/reservations').subscribe(result => {
            this.reservations = result.json() as ReservationExtended[];
        }, error => console.error(error));
    }
}
interface ReservationExtended {
    RoomName: string;
    UserName: string;
    ReservationID: string;
    UserID: string;
    RoomID: string;
    ReservationStartDate: string;
    ReservationEndDate: string;
}