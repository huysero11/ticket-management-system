import type { RootState } from "../../app/store";

export const selectTicketCategoryState = (state: RootState) =>
  state.ticketCategories;

export const selectTicketCategories = (state: RootState) =>
  state.ticketCategories.items;

export const selectSelectedTicketCategory = (state: RootState) =>
  state.ticketCategories.selectedItem;

export const selectTicketCategoryStatus = (state: RootState) =>
  state.ticketCategories.status;

export const selectTicketCategoryMutationStatus = (state: RootState) =>
  state.ticketCategories.mutationStatus;

export const selectTicketCategoryError = (state: RootState) =>
  state.ticketCategories.error;

export const selectIsTicketCategoriesLoading = (state: RootState) =>
  state.ticketCategories.status === "loading";

export const selectIsTicketCategoryMutating = (state: RootState) =>
  state.ticketCategories.mutationStatus === "loading";
