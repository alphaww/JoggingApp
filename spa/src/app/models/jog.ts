import { Time } from "@angular/common";
import { JogLocation } from "./jogLocation";

export class Jog {
    id: string;
    date: Date;
    distance: number;
    time: Time;
    averageSpeed: number;
    location: JogLocation;
}