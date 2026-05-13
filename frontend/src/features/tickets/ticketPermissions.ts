import { TicketStatus } from "./ticketTypes";

export type AppRole = "Admin" | "Agent" | "User";

export const canAssignTicket = (role?: string | null) => {
  return role === "Admin";
};

export const canChangeTicketStatus = (role?: string | null) => {
  return role === "Admin" || role === "Agent";
};

export const canEditTicket = (role?: string | null) => {
  return role === "Admin" || role === "Agent";
};

export const getNextTicketStatuses = (
  currentStatus: TicketStatus,
): TicketStatus[] => {
  switch (currentStatus) {
    case TicketStatus.Open:
      return [TicketStatus.InProgress];

    case TicketStatus.InProgress:
      return [TicketStatus.Resolved];

    case TicketStatus.Resolved:
      return [TicketStatus.Closed, TicketStatus.InProgress];

    case TicketStatus.Closed:
      return [];

    default:
      return [];
  }
};

export const canTicketStatusBeChanged = (
  currentStatus: TicketStatus,
  role?: string | null,
) => {
  return (
    canChangeTicketStatus(role) &&
    getNextTicketStatuses(currentStatus).length > 0
  );
};
