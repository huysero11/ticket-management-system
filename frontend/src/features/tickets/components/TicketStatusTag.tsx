import { Tag } from "antd";
import { TicketStatus, ticketStatusLabels } from "../ticketTypes";

interface TicketStatusTagProps {
  status: TicketStatus;
}

const getStatusColor = (status: TicketStatus): string => {
  switch (status) {
    case TicketStatus.Open:
      return "blue";
    case TicketStatus.InProgress:
      return "purple";
    case TicketStatus.Resolved:
      return "green";
    case TicketStatus.Closed:
      return "default";
    default:
      return "default";
  }
};

export default function TicketStatusTag({ status }: TicketStatusTagProps) {
  return (
    <Tag color={getStatusColor(status)}>
      {ticketStatusLabels[status] ?? "Unknown"}
    </Tag>
  );
}
