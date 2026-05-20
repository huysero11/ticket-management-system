import type { TicketPriority, TicketStatus } from "../tickets/ticketTypes";

export interface DashboardRecentTicket {
    id: string;
    title: string;
    categoryName: string;
    status: TicketStatus;
    priority: TicketPriority;
    createdByUserId: string;
    assignedToUserId: string | null;
    createdAtUtc: string;
}

export interface DashboardSummary {
    totalTickets: number;
    openTickets: number;
    inProgressTickets: number;
    resolvedTickets: number;
    closedTickets: number;
    recentTickets: DashboardRecentTicket[];
}

export interface DashboardState {
    summary: DashboardSummary | null;
    isLoading: boolean;
    error: string | null;
}