export enum TicketStatus {
  Open = 1,
  InProgress = 2,
  Resolved = 3,
  Closed = 4,
}

export enum TicketPriority {
  Low = 1,
  Medium = 2,
  High = 3,
  Critical = 4,
}

export const ticketStatusLabels: Record<TicketStatus, string> = {
  [TicketStatus.Open]: "Open",
  [TicketStatus.InProgress]: "In Progress",
  [TicketStatus.Resolved]: "Resolved",
  [TicketStatus.Closed]: "Closed",
};

export const ticketPriorityLabels: Record<TicketPriority, string> = {
  [TicketPriority.Low]: "Low",
  [TicketPriority.Medium]: "Medium",
  [TicketPriority.High]: "High",
  [TicketPriority.Critical]: "Critical",
};

export interface Ticket {
  id: string;
  title: string;
  description: string;

  categoryId?: string;
  categoryName: string;

  status: TicketStatus;
  priority: TicketPriority;

  createdByUserId: string;
  createdByUserName?: string;

  assignedToUserId: string | null;
  assignedToUserName?: string | null;

  closedAtUtc?: string | null;

  createdAtUtc: string;
  updatedAtUtc: string | null;
}

export interface TicketComment {
  id: string;

  ticketId: string;

  userId: string;
  userName?: string;

  message: string;

  isDeleted?: boolean;
  deletedAtUtc?: string | null;
  deletedByUserId?: string | null;

  createdAtUtc: string;
  updatedAtUtc: string | null;
}

export interface GetTicketsParams {
  pageNumber?: number;
  pageSize?: number;
  status?: TicketStatus | null;
  priority?: TicketPriority | null;
  assignedToUserId?: string | null;
}

export interface CreateTicketRequest {
  title: string;
  description: string;
  categoryId: string;
  priority: TicketPriority;
}

export interface UpdateTicketRequest {
  title: string;
  description: string;
  priority: TicketPriority;
  categoryId: string;
}

export interface AssignTicketRequest {
  assignedToUserId: string;
}

export interface ChangeTicketStatusRequest {
  status: TicketStatus;
}

export interface AddTicketCommentRequest {
  message: string;
}

export interface UpdateTicketCommentRequest {
  message: string;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}

export interface TicketState {
  tickets: Ticket[];
  selectedTicket: Ticket | null;
  comments: TicketComment[];

  pageNumber: number;
  pageSize: number;
  totalCount: number;

  isLoading: boolean;
  isDetailLoading: boolean;
  isSubmitting: boolean;
  isCommentLoading: boolean;

  error: string | null;
}
