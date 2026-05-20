import { createSlice } from "@reduxjs/toolkit";
import type { DashboardState } from "./dashboardTypes";
import { getDashboardSummaryThunk } from "./dashboardThunks";

const initialState: DashboardState = {
    summary: null,
    isLoading: false,
    error: null,
};

const dashboardSlice = createSlice({
    name: "dashboard",
    initialState,
    reducers: {
        clearDashboardError: (state) => {
            state.error = null;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(getDashboardSummaryThunk.pending, (state) => {
                state.isLoading = true;
                state.error = null;
            })
            .addCase(getDashboardSummaryThunk.fulfilled, (state, action) => {
                state.isLoading = false;
                state.summary = action.payload;
            })
            .addCase(getDashboardSummaryThunk.rejected, (state, action) => {
                state.isLoading = false;
                state.error =
                    action.payload?.message ?? "Failed to load dashboard summary.";
            });
    },
});

export const { clearDashboardError } = dashboardSlice.actions;

export default dashboardSlice.reducer;