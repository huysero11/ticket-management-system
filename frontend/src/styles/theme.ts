import type { ThemeConfig } from "antd";

export const appColors = {
  // Main brand colors
  reactBlue: "#087EA4",
  reactBlueHover: "#149ECA",
  reactBlueLight: "#61DAFB",

  reactPurple: "#7C3AED",
  reactPurpleHover: "#8B5CF6",
  reactPurpleLight: "#EDE9FE",

  // Semantic colors
  success: "#52c41a",
  warning: "#faad14",
  error: "#ff4d4f",
  info: "#087EA4",

  // Background colors
  bgLayout: "#f5f7fb",
  bgContainer: "#ffffff",

  // Text colors
  textPrimary: "#1f2937",
  textSecondary: "#6b7280",
  textMuted: "#9ca3af",

  // Border
  border: "#e5e7eb",

  // Layout
  siderBg: "#111827",
};

export const appTypography = {
  fontFamily:
    "Inter, system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif",

  titleLarge: 32,
  titleMedium: 24,
  titleSmall: 20,

  bodyLarge: 16,
  bodyMedium: 14,
  bodySmall: 12,
};

export const appSpacing = {
  xs: 4,
  sm: 8,
  md: 16,
  lg: 24,
  xl: 32,
  xxl: 48,
};

export const appRadius = {
  sm: 6,
  md: 8,
  lg: 12,
  xl: 16,
};

export const antdTheme: ThemeConfig = {
  token: {
    colorPrimary: appColors.reactBlue,
    colorSuccess: appColors.success,
    colorWarning: appColors.warning,
    colorError: appColors.error,
    colorInfo: appColors.info,

    colorLink: appColors.reactBlue,
    colorLinkHover: appColors.reactPurple,

    colorBgLayout: appColors.bgLayout,
    colorBgContainer: appColors.bgContainer,

    colorText: appColors.textPrimary,
    colorTextSecondary: appColors.textSecondary,
    colorBorder: appColors.border,

    fontFamily: appTypography.fontFamily,
    fontSize: appTypography.bodyMedium,

    borderRadius: appRadius.md,
    borderRadiusLG: appRadius.lg,

    controlHeight: 40,
  },

  components: {
    Layout: {
      bodyBg: appColors.bgLayout,
      headerBg: appColors.bgContainer,
      siderBg: appColors.siderBg,
    },

    Menu: {
      darkItemBg: appColors.siderBg,
      darkItemSelectedBg: appColors.reactPurple,
      darkItemSelectedColor: "#ffffff",
    },

    Button: {
      controlHeight: 40,
      borderRadius: appRadius.md,
    },

    Input: {
      controlHeight: 40,
      borderRadius: appRadius.md,
      activeBorderColor: appColors.reactBlue,
      hoverBorderColor: appColors.reactBlueHover,
    },

    Card: {
      borderRadiusLG: appRadius.lg,
    },
  },
};
