import { UserDto } from "../../../core/models/user.model";
import { TagDto } from "./tag.model";

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
  tags: TagDto[];
}

export interface CreateEventDto {
  title: string;
  description: string;
  date: string;
  time: string;
  location: string;
  capacity?: number | null;
  type?: EventType;
  tagNames: string[];
}

export type EventType = 'Public' | 'Private';
