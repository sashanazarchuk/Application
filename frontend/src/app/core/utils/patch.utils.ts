import { CreateEventDto, EventDto } from "../../features/events/models/event.model";

export function createPatchDoc(updated: any, original: any): any[] {
    const patch: any[] = [];
    Object.keys(updated).forEach(key => {
        if (updated[key] !== original[key]) {
            patch.push({ op: 'replace', path: `/${key}`, value: updated[key] });
        }
    });
    return patch;
}

export function combineDateTime(date: Date, time: Date): string {
    const combined = new Date(date);
    combined.setHours(time.getHours(), time.getMinutes(), 0, 0);  
    return combined.toISOString().slice(0, 16) + 'Z';  
}

export function mapEventToForm(event: EventDto): CreateEventDto {
    return {
        title: event.title,
        description: event.description,
        date: event.date.split('T')[0],
        time: event.date.split('T')[1].slice(0, 5),
        location: event.location,
        capacity: event.capacity ?? null,
        type: event.type
    };
}


export function buildPatchDoc(updatedData: CreateEventDto, originalEvent: EventDto): any[] {
    const dateISO = combineDateTime(new Date(updatedData.date), new Date(`1970-01-01T${updatedData.time}:00`));
    const updatedForPatch: Partial<EventDto> = { ...updatedData, date: dateISO };
    delete (updatedForPatch as any).time;
    return createPatchDoc(updatedForPatch, originalEvent);
}