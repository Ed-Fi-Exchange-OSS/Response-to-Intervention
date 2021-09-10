<template>
  <div>
    <v-data-table
      :headers="headers"
      :items="items"
      :loading="isLoading"
      :options.sync="listViewOptions"
      :server-items-length="100"
      :footer-props="{ 'items-per-page-options': [10, 20, 30, 40, 50] }">
      <template v-slot:header.studentName="{ header }">
        <v-btn
          small
          text
          class="text-capitalize"
          @click="showDynamicSlots">
          {{ header.text }}
        </v-btn>
      </template>

      <template v-slot:item.studentName="{ item }">
        <v-chip
          small
          color="primary"
          @click="viewStudent(item)">
          {{ item.studentName }}
        </v-chip>
      </template>

      <!-- dynamic item slots (for intervention columns) -->
      <template
        v-for="slot in dynamicSlots"
        v-slot:[slot.name]="{ item }">
        <v-simple-checkbox
          :key="slot.name"
          :ripple="false"
          :color="getSlotColor(slot)"
          :value="item[slot.field]"
          @click="onDynamicSlotClicked(item, slot)" />
      </template><!-- dynamic item slots (for intervention columns) -->
    </v-data-table>

    <!-- dialogs -->
    <div>
      <StudentProfileDialog ref="studentProfileDialog" />
    </div><!-- dialogs -->
  </div>
</template>

<script>
/* eslint-disable */

import StudentProfileDialog from '../dialogs/StudentProfileDialog';
import { DynamicSlotStatus, DynamicSlotColors } from '../../models/constants/DynamicSlots';

export default {

    props: {
        headers: Array,
        items: Array,
        isLoading: Boolean,
        searchParams: Object,
    }, // props

    data() {
        return {
            listViewOptions: {
                page: 1,
                itemsPerPage: 10,
            },

            dynamicSlots: [],
        };
    }, // data

    mounted() {

    }, // mounted

    methods: {
        getSlotColor(slot) {
            if (!slot.status) return 'gray';
            return DynamicSlotColors[slot.status];
        },

        /**
         * Row and slot changes must be notified to parent component to keep
         * track of state history in order to undo/redo changes
         */
        onDynamicSlotClicked(item, slot) {
            // Copy states to avoid passing original to parent
            let itemCopy = JSON.parse(JSON.stringify(item));
            let slotCopy = JSON.parse(JSON.stringify(slot));
            let itemState = { item: itemCopy, slot: slotCopy };

            if (!slot.status || slot.status == DynamicSlotStatus.committed) {
                slot.status = DynamicSlotStatus.dirty;
                slot.color = DynamicSlotColors.dirty;
            }

            this.$emit('row-changed', itemState);

            // Update actual value AFTER passing to parent
            item[slot.field] = !item[slot.field];
        },

        onNewHeaderAdded(header) {
            let newSlot = {
                name: `item.${header.value}`,
                field: header.value,
            };

            this.dynamicSlots.push(newSlot);
        },

        showDynamicSlots() {
            console.log('Dynamic Slots', this.dynamicSlots);
        },

        viewStudent(item) {
            this.$refs.studentProfileDialog.open();
        },
    }, // methods

    watch: {
        /**
         * Changes for headers must be watched to keep sync with the corresponding dyanmic slots.
         * In this specific case, it is supposed that only new headers are added, not deleted/removed.
         */
        headers() {
            let newHeader = this.headers[this.headers.length - 1];
            this.onNewHeaderAdded(newHeader);
        },
    },

    components: {
        StudentProfileDialog,
    }, // components
}
</script>
