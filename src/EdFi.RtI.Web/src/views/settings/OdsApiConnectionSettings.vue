<template>
  <div>

    <v-card>
      <v-card-text>
        <OdsApiSettingsForm :edFiVersion="edFiVersion" :loading="loadingOdsApiSettings" :odsApiSettings="odsApiSettings" @edFiVersionChanged="onEdFiVersionChanged"/>
      </v-card-text>
      <v-card-actions>
        <v-spacer/>
        <v-btn v-if="isEdFiVersionSelected && !loadingOdsApiSettings" class="mb-2 mr-2" color="accent" @click="onSetDefaultEdFiVersionClicked">Set {{edFiVersionDescription}} as my default Ed-Fi version</v-btn>
      </v-card-actions>
    </v-card>

    <AlertDialog ref="alertDialog"/>
    <LoadingDialog ref="loadingDialog"/>

  </div>
</template>

<script>
import { mapActions } from "vuex";
import AppToolbar from "@/components/widgets/AppToolbar.vue";
import { EdFiOdsApiSettings } from '@/models/EdFiOdsApiSettings'
import api from '@/api'
import timers from '@/utils/timers';
import { edFiVersionSuite2, edFiVersionSuite3 } from '@/models/EdFiVersion';
import storage from '@/storage';

export default {

  data: () => ({
    edFiVersion: '',
    odsApiSettings: new EdFiOdsApiSettings(),
    loadingOdsApiSettings: false,
  }), // data

  mounted() {
    this.init()
  }, // mounted

  computed: {
    alertDialog() {
      return this.$refs.alertDialog
    },

    loadingDialog() {
      return this.$refs.loadingDialog
    },

    isEdFiVersionSelected() {
      return this.edFiVersion != undefined && this.edFiVersion != null && this.edFiVersion.trim().length != 0
    },

    edFiVersionDescription() {
      if (this.edFiVersion == edFiVersionSuite2.value)
        return edFiVersionSuite2.name

      if (this.edFiVersion == edFiVersionSuite3.value)
        return edFiVersionSuite3.name

      throw `Invalid Ed-Fi version ${this.edFiVersion}`
    }
  }, // computed

  methods: {
    async init() {
      await this.getDefaultEdFiVersion()
      await this.getOdsApiSettings()
    },

    async getDefaultEdFiVersion() {
      this.loadingOdsApiSettings = true

      try {
        this.edFiVersion = await api.settings.getDefaultEdFiVersion()
      } finally {
        this.loadingOdsApiSettings = false
      }
    },

    async getOdsApiSettings() {
      this.loadingOdsApiSettings = true
      this.odsApiSettings = new EdFiOdsApiSettings()

      try {
        await timers.wait(1000)
        this.odsApiSettings = await api.settings.getOdsApiSettings(this.edFiVersion)
      } finally {
        this.loadingOdsApiSettings = false
      }
    },

    onEdFiVersionChanged(edFiVersion) {
      this.edFiVersion = edFiVersion
      this.getOdsApiSettings()
    },

    async onSetDefaultEdFiVersionClicked() {
      this.loadingDialog.open({ title: 'Setting default Ed-Fi version' })

      try {
        await timers.wait(1000)
        await api.settings.setDefaultEdFiVersion(this.edFiVersion)
        storage.local.setEdFiVersion(this.edFiVersion)
        await timers.wait(500)
        window.location.reload()
      } finally {
        this.loadingDialog.close()
      }
    }
  }, // methods

  components: {
    AppToolbar,
  }, // components

};
</script>
