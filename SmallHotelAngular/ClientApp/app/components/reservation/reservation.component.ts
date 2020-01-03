import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { forEach } from '@angular/router/src/utils/collection';
@Component({
    selector: 'reservation',
    templateUrl: './reservation.component.html'
})
export class ReservationComponent {
    public res: ReservationExtended[];
    public selectedRoom: Room;
    public allRooms: Room[];
    public selectedRoomId: string;  
    public selectedImgSrc: string;  
    public selectedRoomDescription: string; 
    public email: string;
    public btnReservationVisible: boolean;
    public reservationState: string;
    public isDateRangeOK: boolean;
    myhttp: Http;
    webServiceBaseUrl: string;
    public webServerQuery = "- webServerQuery -";

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this.myhttp = http;
        this.webServiceBaseUrl = baseUrl;
        this.loadAllRooms();
        this.btnReservationVisible = false;
        this.reservationState = "Make a reservation by click here";
        this.isDateRangeOK = false;
    }

    public loadAvaliableRooms(fullUrl: string): void {
        this.myhttp.get(fullUrl).subscribe(result => {
            this.res = result.json() as ReservationExtended[];
        }, error => console.error(error));
    }

    public loadAllRooms(): void {
        this.myhttp.get(this.webServiceBaseUrl + "api/rooms").subscribe(result => {
            this.allRooms = result.json() as Room[];
        }, error => console.error(error));
    }

    public getSelectedRoom(roomID: string): Room {
        var result: Room = {} as Room;     
        this.allRooms.forEach(function (room) {
            if (room.RoomID.toString() == roomID) { result = room; }
        });
        return result;
    }

    public getAvalilableRooms(){
        try {
            var dStart = new Date(((<HTMLInputElement>document.getElementById("dateStartDate")).value).toString());
            var dEnd = new Date(((<HTMLInputElement>document.getElementById("dateEndDate")).value).toString());
            var sYear: number = dStart.getFullYear();
            var sMonth: number = dStart.getMonth() + 1;
            var sDay: number = dStart.getDate();
            var eYear: number = dEnd.getFullYear();
            var eMonth: number = dEnd.getMonth() + 1;
            var eDay: number = dEnd.getDate();
            this.webServerQuery = this.webServiceBaseUrl + "api/reservations/" + sYear + "/" + sMonth + "/" + sDay + "/" + eYear + "/" + eMonth + "/" + eDay;
            this.loadAvaliableRooms(this.webServerQuery);
        } catch (e) {}
    }

    public makeReservation(): string {   
        var dStart = new Date(((<HTMLInputElement>document.getElementById("dateStartDate")).value).toString());
        var dEnd = new Date(((<HTMLInputElement>document.getElementById("dateEndDate")).value).toString());
        var sYear: number = dStart.getFullYear();
        var sMonth: number = dStart.getMonth() + 1;
        var sDay: number = dStart.getDate();
        var eYear: number = dEnd.getFullYear();
        var eMonth: number = dEnd.getMonth() + 1;
        var eDay: number = dEnd.getDate();

        this.myhttp.get(this.webServiceBaseUrl + "api/reservations/"
            + this.email + "/"
            + this.selectedRoomId + "/"
            + sYear + "/" + sMonth + "/" + sDay + "/" + eYear + "/" + eMonth + "/" + eDay
        ).subscribe(result => {
            this.reservationState = (result.json() as String).toString();
            }, error => console.error(error));
        return this.reservationState;
    }

    public onMakeReservation() {
        this.makeReservation();
    }

    public isValidPeriod(): boolean {
        var dStart = new Date(((<HTMLInputElement>document.getElementById("dateStartDate")).value).toString());
        var dEnd = new Date(((<HTMLInputElement>document.getElementById("dateEndDate")).value).toString());
        if (dStart > dEnd) return false;
        if (!isNaN(Date.parse(dStart.toString())) && !isNaN(Date.parse(dEnd.toString()))) {
            return true;
        }
        return false;
    }

    private validatePeriod()
    {
        if (this.isValidPeriod()) {
            this.isDateRangeOK = true;
            this.getAvalilableRooms();
        }
        else { this.isDateRangeOK = false;}
    }

    public onDateStartChanged()
    {
        this.validatePeriod();
    }

    public onDateEndChanged()
    {
        this.validatePeriod();
    }

    public checkEmail() {
        try {
            this.email = ((<HTMLInputElement>document.getElementById("txtEmail")).value).toString();
        } catch (e) {
            this.email = "err";
        }   

        if (this.isEmail(this.email)) {
            this.btnReservationVisible = true;
        } else {
            this.btnReservationVisible = false;
        }
        
    }

    public isEmail(email: string): boolean {
        var regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
        return regex.test(email);
    } 

    public onChange(deviceValue: string) {
        var splitted = deviceValue.split(" ", 2);
        this.selectedRoomId = splitted[1];
        this.selectedImgSrc = this.selectedRoomId + ".jpg"
        this.selectedRoomDescription = "Working on it... :)";
        this.selectedRoom = this.getSelectedRoom(this.selectedRoomId);
        this.selectedRoomDescription = this.selectedRoom.RoomName + " has capacity of " + this.selectedRoom.RoomCapacity + ".";
        if (this.selectedRoom.RoomMinibar) this.selectedRoomDescription += " With minibar.";
        if (this.selectedRoom.RoomWiFi) this.selectedRoomDescription += " WiFi available.";
        this.selectedRoomDescription += " Make a reservation now!";
    }
}

interface ReservationExtended {
    RoomName: string;
    RoomID: string;
}

interface Room {
    RoomID: number;
    RoomName: string;
    RoomCapacity: number;
    RoomWiFi: boolean;
    RoomMinibar: boolean;
}