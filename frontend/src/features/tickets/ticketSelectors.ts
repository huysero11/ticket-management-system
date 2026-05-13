import { createSelector } from "@reduxjs/toolkit";
import type { RootState } from "../../app/store";

export const selectTicketState = (state: RootState) => state.tickets;

export const selectTickets = (state: RootState) => state.tickets.tickets;

export const selectSelectedTicket = (state: RootState) =>
  state.tickets.selectedTicket;

export const selectTicketComments = (state: RootState) =>
  state.tickets.comments;

export const selectTicketLoading = (state: RootState) =>
  state.tickets.isLoading;

export const selectTicketDetailLoading = (state: RootState) =>
  state.tickets.isDetailLoading;

export const selectTicketSubmitting = (state: RootState) =>
  state.tickets.isSubmitting;

export const selectTicketCommentLoading = (state: RootState) =>
  state.tickets.isCommentLoading;

export const selectTicketError = (state: RootState) => state.tickets.error;

export const selectTicketPagination = createSelector(
  [selectTicketState],
  (ticketState) => ({
    pageNumber: ticketState.pageNumber,
    pageSize: ticketState.pageSize,
    totalCount: ticketState.totalCount,
  }),
);
