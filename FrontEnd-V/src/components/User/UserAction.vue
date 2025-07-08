<template>
  <v-dialog v-model="isOpen" max-width="400px" persistent>
    <v-card>
      <v-card-title class="bg-surface-light pt-4" v-if="action === 1">Activar Item?</v-card-title>
      <v-card-title class="bg-surface-light pt-4" v-if="action === 2">Desactivar Item?</v-card-title>
      <v-divider></v-divider>
      <v-card-text>
        Estás a punto de <span v-if="action === 1">Activar</span>
        <span v-if="action === 2">Desactivar</span> el ítem {{ localUser.useR_NAME }}.
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
    user: {
      type: Object,
      default: () => ({
        pK_USER: null,
        useR_NAME: ''
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
      localUser: { ...this.user },
    };
  },
  watch: {
    modelValue(newValue) {
      this.isOpen = newValue;
    },
    user: {
      handler(newUser) {
        this.localUser = { ...newUser };
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
      try{
        await this.$store.dispatch('user/enabledUser', this.localUser.pK_USER);
      this.toast.success('Usuario habilitado con éxito!');
      this.close();
      } catch (error) {
        if (error.response) {
          this.toast.error('Error al habilitar el Usuario.');
        }
      }
    },
    async disabled() {
      try{
        await this.$store.dispatch('user/disabledUser', this.localUser.pK_USER);
      this.toast.success('Usuario deshabilitado con éxito!');
      this.close();
      } catch (error) {
        if (error.response) {
          this.toast.error('Error al deshabilitar el Usuario.');
        }
      }
    },
  },
};
</script>