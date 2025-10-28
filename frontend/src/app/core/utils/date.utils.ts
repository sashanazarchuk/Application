export function combineDateTime(date: string | Date, time: string | Date): string {
    const d = typeof date === 'string' ? parseDateString(date) : date;
    const t = typeof time === 'string' ? parseTimeString(time) : { hours: time.getHours(), minutes: time.getMinutes() };

    d.setHours(t.hours, t.minutes, 0, 0);
    return d.toISOString();
}

function parseDateString(date: string): Date {
    const [year, month, day] = date.split('-').map(Number);
    return new Date(year, month - 1, day);
}

function parseTimeString(time: string): { hours: number, minutes: number } {
    const [hours, minutes] = time.split(':').map(Number);
    return { hours, minutes };
}