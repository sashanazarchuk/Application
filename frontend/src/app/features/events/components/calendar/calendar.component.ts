import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { FullCalendarComponent, FullCalendarModule } from '@fullcalendar/angular';
import { CalendarOptions } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';

@Component({
  selector: 'app-calendar-component',
  imports: [CommonModule, FullCalendarModule],
  templateUrl: './calendar.component.html',
})

export class CalendarComponent {

  @Input() events: any[] = [];
  @Output() eventClick = new EventEmitter<any>();

  @ViewChild('calendar') calendarComponent!: FullCalendarComponent;

  calendarTitle = '';
  currentView = 'month';

  constructor(private cdr: ChangeDetectorRef) { }

  calendarOptions: CalendarOptions = {
    initialView: 'dayGridMonth',
    plugins: [dayGridPlugin, interactionPlugin],
    events: this.events,

    headerToolbar: {
      left: '',
      center: '',
      right: ''
    },

    eventTimeFormat: {
      hour: '2-digit',
      minute: '2-digit',
      hour12: false
    },

    eventBackgroundColor: '#9c2bff',

    datesSet: (arg) => { this.calendarTitle = arg.view.title; this.cdr.detectChanges(); },
    height: 'auto',

    eventClick: (clickInfo) => {
      this.eventClick.emit(clickInfo.event); 
    }

  };

  ngOnChanges() {
    this.calendarOptions.events = this.events;
  }

  setView(view: 'month' | 'week') {
    const api = this.calendarComponent?.getApi();
    api?.changeView(view === 'month' ? 'dayGridMonth' : 'dayGridWeek');
    this.currentView = view;
  }
}