import { Tag } from "antd";
import { TicketPriority, ticketPriorityLabels } from "../ticketTypes";

interface TicketPriorityTagProps {
  priority: TicketPriority;
}

const getPriorityColor = (priority: TicketPriority): string => {
  switch (priority) {
    case TicketPriority.Low:
      return "default";
    case TicketPriority.Medium:
      return "blue";
    case TicketPriority.High:
      return "orange";
    case TicketPriority.Critical:
      return "red";
    default:
      return "default";
  }
};

export default function TicketPriorityTag({
  priority,
}: TicketPriorityTagProps) {
  return (
    <Tag color={getPriorityColor(priority)}>
      {ticketPriorityLabels[priority] ?? "Unknown"}
    </Tag>
  );
}
