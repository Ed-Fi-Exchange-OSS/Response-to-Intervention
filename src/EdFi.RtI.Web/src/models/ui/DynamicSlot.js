import { DynamicSlotColors, DynamicSlotStatus } from "../constants/DynamicSlots"

export class DynamicSlot {
    name = "";  // The name used for the template v-slot

    field = ""; // The Intervention field that this slot represents

    status = DynamicSlotStatus.committed;

    color = DynamicSlotColors.committed;
};
