import type { AppError, RequestStatus } from "../../types/common";

export interface TicketCategory {
  id: string;
  name: string;
  description?: string | null;
  isActive?: boolean;
  createdAtUtc?: string;
  updatedAtUtc?: string | null;
}

export interface CreateTicketCategoryRequest {
  name: string;
  description?: string | null;
}

export interface UpdateTicketCategoryRequest {
  name: string;
  description?: string | null;
}

export interface UpdateTicketCategoryPayload {
  id: string;
  request: UpdateTicketCategoryRequest;
}

export interface TicketCategoryState {
  items: TicketCategory[];
  selectedItem: TicketCategory | null;
  status: RequestStatus;
  mutationStatus: RequestStatus;
  error: AppError | null;
}
