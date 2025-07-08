<template>
  <v-dialog v-model="isOpen" max-width="400px" persistent>
    <v-card>
      <v-card-title class="bg-surface-light pt-4" v-if="action === 1">Activar Item?</v-card-title>
      <v-card-title class="bg-surface-light pt-4" v-if="action === 2">Desactivar Item?</v-card-title>
      <v-divider></v-divider>
      <v-card-text>
        Estás a punto de <span v-if="action === 1">Activar</span>
        <span v-if="action === 2">Desactivar</span> el ítem {{ localBrand.branD_NAME }}.
      </v-card-text>
      <v-card-actions class="d-flex justify-space-between">
        <div class="d-flex">
          <v-btn v-if="action === 1" color="blue" dark class="mr-2" elevation="4" @click="enabled">Activar</v-btn>
          <v-btn v-if="action === 2" color="blue" dark class="mr-2" elevation="4" @click="disabled">Desactivar</v-btn>
          <v-btn color="red" elevation="4" @click="close">Cancelar</v-btn>
        </div>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import { useToast } from 'vue-toastification';
export default {
  props: {
    modelValue: {
      type: Boolean,
      required: true,
    },
    brand: {
      type: Object,
      default: () => ({
        pK_BRAND: null,
        branD_NAME: ''
      }),
    },
    action: {
      type: Number,
      required: true,
    },
  },
  data() {
    return {
      isOpen: this.modelValue,
      valid: false,
      toast: useToast(),
      localBrand: { ...this.brand },
    };
  },
  watch: {
    modelValue(newValue) {
      this.isOpen = newValue;
    },
    brand: {
      handler(newBrand) {
        this.localBrand = { ...newBrand };
      },
      deep: true,
    },
  },
  methods: {
    close() {
      this.isOpen = false;
      this.$emit('update:modelValue', false);
    },
    async enabled() {
      try {
        await this.$store.dispatch('brand/enabledBrand', this.localBrand.pK_BRAND);
        this.toast.success('Marca habilitada con éxito!');
        this.close();
      } catch (error) {
        if (error.response) {
          this.toast.error('Error al habilitar la Marca.');
        }
      }
    },
    async disabled() {
      try {
        await this.$store.dispatch('brand/disabledBrand', this.localBrand.pK_BRAND);
        this.toast.success('Marca deshabilitada con éxito!');
        this.close();
      } catch (error) {
        if (error.response) {
          this.toast.error('Error al deshabilitar la Marca.');
        }
      }
    },
  },
};
</script>