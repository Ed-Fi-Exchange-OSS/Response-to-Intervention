<template>
  <div>
    <CachingListView> </CachingListView>
  </div>
</template>

<script>
import AppToolbar from "@/components/widgets/AppToolbar.vue";
import CachingListView from "./CachingListView";

export default {
  data() {
    return {
      searchInput: "",
      toolbarTitle: "Caching",
      toolbarIcon: "mdi-cached",
      buttons: [
        {
          icon: "mdi-rotate-right",
          text: "Refresh All",
          click: () => {
            this.refresh();
          },
        },
      ],
    };
  }, // data

  methods: {
    refresh() {
      console.log("Refresh all cache");
    },
  },

  components: {
    AppToolbar,
    CachingListView,
  },
};
</script>