import { createSlice } from "@reduxjs/toolkit";
import type { TicketState } from "./ticketTypes";
import {
  addTicketCommentThunk,
  assignTicketThunk,
  changeTicketStatusThunk,
  createTicketThunk,
  deleteTicketCommentThunk,
  getTicketByIdThunk,
  getTicketCommentsThunk,
  getTicketsThunk,
  updateTicketThunk,
  updateTicketCommentThunk,
} from "./ticketThunks";

const initialState: TicketState = {
  tickets: [],
  selectedTicket: null,
  comments: [],

  pageNumber: 1,
  pageSize: 10,
  totalCount: 0,

  isLoading: false,
  isDetailLoading: false,
  isSubmitting: false,
  isCommentLoading: false,

  error: null,
};

const ticketSlice = createSlice({
  name: "tickets",
  initialState,
  reducers: {
    clearTicketError: (state) => {
      state.error = null;
    },

    clearSelectedTicket: (state) => {
      state.selectedTicket = null;
      state.comments = [];
    },
  },
  extraReducers: (builder) => {
    builder
      // Get tickets
      .addCase(getTicketsThunk.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(getTicketsThunk.fulfilled, (state, action) => {
        state.isLoading = false;

        state.tickets = action.payload.items;
        state.totalCount = action.payload.totalCount;
        state.pageNumber = action.payload.pageNumber;
        state.pageSize = action.payload.pageSize;
      })
      .addCase(getTicketsThunk.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload?.message ?? "Failed to load tickets.";
      })

      // Get ticket by id
      .addCase(getTicketByIdThunk.pending, (state) => {
        state.isDetailLoading = true;
        state.error = null;
      })
      .addCase(getTicketByIdThunk.fulfilled, (state, action) => {
        state.isDetailLoading = false;
        state.selectedTicket = action.payload;
      })
      .addCase(getTicketByIdThunk.rejected, (state, action) => {
        state.isDetailLoading = false;
        state.error = action.payload?.message ?? "Failed to load ticket.";
      })

      // Create ticket
      .addCase(createTicketThunk.pending, (state) => {
        state.isSubmitting = true;
        state.error = null;
      })
      .addCase(createTicketThunk.fulfilled, (state) => {
        state.isSubmitting = false;
      })
      .addCase(createTicketThunk.rejected, (state, action) => {
        state.isSubmitting = false;
        state.error = action.payload?.message ?? "Failed to create ticket.";
      })

      // Update ticket
      .addCase(updateTicketThunk.pending, (state) => {
        state.isSubmitting = true;
        state.error = null;
      })
      .addCase(updateTicketThunk.fulfilled, (state) => {
        state.isSubmitting = false;
      })
      .addCase(updateTicketThunk.rejected, (state, action) => {
        state.isSubmitting = false;
        state.error = action.payload?.message ?? "Failed to update ticket.";
      })

      // Assign ticket
      .addCase(assignTicketThunk.pending, (state) => {
        state.isSubmitting = true;
        state.error = null;
      })
      .addCase(assignTicketThunk.fulfilled, (state) => {
        state.isSubmitting = false;
      })
      .addCase(assignTicketThunk.rejected, (state, action) => {
        state.isSubmitting = false;
        state.error = action.payload?.message ?? "Failed to assign ticket.";
      })

      // Change status
      .addCase(changeTicketStatusThunk.pending, (state) => {
        state.isSubmitting = true;
        state.error = null;
      })
      .addCase(changeTicketStatusThunk.fulfilled, (state) => {
        state.isSubmitting = false;
      })
      .addCase(changeTicketStatusThunk.rejected, (state, action) => {
        state.isSubmitting = false;
        state.error =
          action.payload?.message ?? "Failed to change ticket status.";
      })

      // Get comments
      .addCase(getTicketCommentsThunk.pending, (state) => {
        state.isCommentLoading = true;
        state.error = null;
      })
      .addCase(getTicketCommentsThunk.fulfilled, (state, action) => {
        state.isCommentLoading = false;
        state.comments = action.payload;
      })
      .addCase(getTicketCommentsThunk.rejected, (state, action) => {
        state.isCommentLoading = false;
        state.error =
          action.payload?.message ?? "Failed to load ticket comments.";
      })

      // Add comment
      .addCase(addTicketCommentThunk.pending, (state) => {
        state.isCommentLoading = true;
        state.error = null;
      })
      .addCase(addTicketCommentThunk.fulfilled, (state, action) => {
        state.isCommentLoading = false;
        state.comments.unshift(action.payload);
      })
      .addCase(addTicketCommentThunk.rejected, (state, action) => {
        state.isCommentLoading = false;
        state.error = action.payload?.message ?? "Failed to add comment.";
      })

      // Update comment
      .addCase(updateTicketCommentThunk.pending, (state) => {
        state.isCommentLoading = true;
        state.error = null;
      })
      .addCase(updateTicketCommentThunk.fulfilled, (state, action) => {
        state.isCommentLoading = false;

        const index = state.comments.findIndex(
          (comment) => comment.id === action.payload.id,
        );

        if (index !== -1) {
          state.comments[index] = action.payload;
        }
      })
      .addCase(updateTicketCommentThunk.rejected, (state, action) => {
        state.isCommentLoading = false;
        state.error = action.payload?.message ?? "Failed to update comment.";
      })

      // Delete comment
      .addCase(deleteTicketCommentThunk.pending, (state) => {
        state.isCommentLoading = true;
        state.error = null;
      })
      .addCase(deleteTicketCommentThunk.fulfilled, (state, action) => {
        state.isCommentLoading = false;

        state.comments = state.comments.filter(
          (comment) => comment.id !== action.payload,
        );
      })
      .addCase(deleteTicketCommentThunk.rejected, (state, action) => {
        state.isCommentLoading = false;
        state.error = action.payload?.message ?? "Failed to delete comment.";
      });
  },
});

export const { clearTicketError, clearSelectedTicket } = ticketSlice.actions;

export default ticketSlice.reducer;
