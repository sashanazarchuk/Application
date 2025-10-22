export interface EventDto {
  id: string;
  title: string;
  description: string;
  date: string;
  location: string;
  capacity?: number | null;
  currentParticipantsCount: number;
  isFull?: boolean;
  isJoined?: boolean;
  isAdmin?: boolean;
  participants?: UserDto[];
  type?: EventType;
}

export interface UserDto {
  id: string;
  fullName: string;
}


export interface CreateEventDto {
  title: string;
  description: string;
  date: string;
  time: string;
  location: string;
  capacity?: number | null;
  type?: EventType;
}

export type EventType = 'Public' | 'Private';
