import type { RootState } from "../../app/store";

export const selectDashboardSummary = (state: RootState) =>
    state.dashboard.summary;

export const selectDashboardLoading = (state: RootState) =>
    state.dashboard.isLoading;

export const selectDashboardError = (state: RootState) =>
    state.dashboard.error;