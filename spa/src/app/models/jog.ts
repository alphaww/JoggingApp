import { Time } from "@angular/common";
import { JogLocation } from "./jogLocation";
import { RunningTime } from "./runningTime";

export class Jog {
    id: string;
    date: Date;
    distance: number;
    formattedTime: string;
    time: RunningTime;
    averageSpeed: number;
    location: JogLocation;
}