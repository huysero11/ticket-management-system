import { createSlice } from "@reduxjs/toolkit";
import type { TicketCategoryState } from "./ticketCategoryTypes";
import {
  createTicketCategoryThunk,
  deleteTicketCategoryThunk,
  getTicketCategoriesThunk,
  getTicketCategoryByIdThunk,
  updateTicketCategoryThunk,
} from "./ticketCategoryThunks";

const initialState: TicketCategoryState = {
  items: [],
  selectedItem: null,
  status: "idle",
  mutationStatus: "idle",
  error: null,
};

const ticketCategorySlice = createSlice({
  name: "ticketCategories",
  initialState,
  reducers: {
    clearSelectedTicketCategory: (state) => {
      state.selectedItem = null;
    },
    clearTicketCategoryError: (state) => {
      state.error = null;
    },
    resetTicketCategoryMutationStatus: (state) => {
      state.mutationStatus = "idle";
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(getTicketCategoriesThunk.pending, (state) => {
        state.status = "loading";
        state.error = null;
      })
      .addCase(getTicketCategoriesThunk.fulfilled, (state, action) => {
        state.status = "succeeded";
        state.items = action.payload;
      })
      .addCase(getTicketCategoriesThunk.rejected, (state, action) => {
        state.status = "failed";
        state.error = action.payload ?? {
          message: "Failed to get ticket categories.",
        };
      });

    builder
      .addCase(getTicketCategoryByIdThunk.pending, (state) => {
        state.status = "loading";
        state.error = null;
      })
      .addCase(getTicketCategoryByIdThunk.fulfilled, (state, action) => {
        state.status = "succeeded";
        state.selectedItem = action.payload;
      })
      .addCase(getTicketCategoryByIdThunk.rejected, (state, action) => {
        state.status = "failed";
        state.error = action.payload ?? {
          message: "Failed to get ticket category.",
        };
      });

    builder
      .addCase(createTicketCategoryThunk.pending, (state) => {
        state.mutationStatus = "loading";
        state.error = null;
      })
      .addCase(createTicketCategoryThunk.fulfilled, (state, action) => {
        state.mutationStatus = "succeeded";
        state.items.unshift(action.payload);
      })
      .addCase(createTicketCategoryThunk.rejected, (state, action) => {
        state.mutationStatus = "failed";
        state.error = action.payload ?? {
          message: "Failed to create ticket category.",
        };
      });

    builder
      .addCase(updateTicketCategoryThunk.pending, (state) => {
        state.mutationStatus = "loading";
        state.error = null;
      })
      .addCase(updateTicketCategoryThunk.fulfilled, (state, action) => {
        state.mutationStatus = "succeeded";

        const { id, request } = action.meta.arg;

        const index = state.items.findIndex((item) => item.id === id);

        if (index !== -1) {
          state.items[index] = {
            ...state.items[index],
            ...request,
          };
        }
      })
      .addCase(updateTicketCategoryThunk.rejected, (state, action) => {
        state.mutationStatus = "failed";
        state.error = action.payload ?? {
          message: "Failed to update ticket category.",
        };
      });

    builder
      .addCase(deleteTicketCategoryThunk.pending, (state) => {
        state.mutationStatus = "loading";
        state.error = null;
      })
      .addCase(deleteTicketCategoryThunk.fulfilled, (state, action) => {
        state.mutationStatus = "succeeded";

        const deletedId = action.meta.arg;

        state.items = state.items.filter((item) => item.id !== deletedId);
      })
      .addCase(deleteTicketCategoryThunk.rejected, (state, action) => {
        state.mutationStatus = "failed";
        state.error = action.payload ?? {
          message: "Failed to delete ticket category.",
        };
      });
  },
});

export const {
  clearSelectedTicketCategory,
  clearTicketCategoryError,
  resetTicketCategoryMutationStatus,
} = ticketCategorySlice.actions;

export default ticketCategorySlice.reducer;
