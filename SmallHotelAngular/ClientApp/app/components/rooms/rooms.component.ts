import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
@Component({
    selector: 'rooms',
    templateUrl: './rooms.component.html'
})
export class RoomsComponent {
    public rooms: Room[];
    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        http.get(baseUrl + 'api/rooms').subscribe(result => {
            this.rooms = result.json() as Room[];
        }, error => console.error(error));
    }
}
interface Room {
    RoomID: string;
    RoomName: string;
    RoomCapacity: string;
    RoomWiFi: string;
    RoomMinibar: string;
}