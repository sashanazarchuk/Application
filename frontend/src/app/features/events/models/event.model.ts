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
}