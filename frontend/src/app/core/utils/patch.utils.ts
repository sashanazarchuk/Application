import { CreateEventDto, EventDto } from "../../features/events/models/event.model";
import { combineDateTime } from "./date.utils";

export function createPatchDoc(updated: any, original: any): any[] {
    const patch: any[] = [];
    Object.keys(updated).forEach(key => {
        if (updated[key] !== original[key]) {
            patch.push({ op: 'replace', path: `/${key}`, value: updated[key] });
        }
    });
    return patch;
}

export function mapEventToForm(event: EventDto): CreateEventDto {
    const dateObj = new Date(event.date);
    const dateStr = dateObj.toISOString().slice(0, 10);
    const timeStr = dateObj.toLocaleTimeString('en-GB', { hour12: false, hour: '2-digit', minute: '2-digit' });
    return {
        title: event.title,
        description: event.description,
        date: dateStr,
        time: timeStr,
        location: event.location,
        capacity: event.capacity ?? null,
        type: event.type,
        tagNames: event.tags.map(tag => tag.name)
    };
}

export function buildPatchDoc(updatedData: CreateEventDto, originalEvent: EventDto): any[] {
    const dateISO = combineDateTime(new Date(updatedData.date), new Date(`1970-01-01T${updatedData.time}:00`));
    const updatedForPatch: Partial<EventDto> = { ...updatedData, date: dateISO };
    delete (updatedForPatch as any).time;
    return createPatchDoc(updatedForPatch, originalEvent);
}