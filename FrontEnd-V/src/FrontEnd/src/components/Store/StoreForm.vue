<template>
  <v-dialog v-model="isOpen" max-width="500px" persistent>
    <v-card>
      <v-card-title class="bg-surface-light pt-4">
        <span>{{ localStore.pK_STORE ? 'Editar Sucursal' : 'Agregar Sucursal' }}</span>
      </v-card-title>
      <v-divider></v-divider>
      <v-card-text>
        <v-form ref="form" v-model="valid">
          <v-container>
            <v-row>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localStore.storE_NAME"
                  :rules="[rules.required]" counter="30" :maxlength="30" @keyup="uppercase"
                  label="Nombre de la Sucursal" required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localStore.manager"
                  :rules="[rules.required]" counter="50" :maxlength="50" @keyup="uppercase" label="Encargado"
                  required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localStore.address"
                  :rules="[rules.required]" counter="50" :maxlength="50" @keyup="uppercase" label="Dirección"
                  required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localStore.phone" counter="8" :maxlength="8"
                  onkeypress="return (event.charCode >= 48 && event.charCode <= 57)" :rules="[rules.required]"
                  label="Teléfono" required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localStore.city" :rules="[rules.required]"
                  counter="20" :maxlength="20" @keyup="uppercase" label="Ciudad" required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localStore.email" counter="50"
                  :maxlength="50" @keyup="uppercase" label="Correo" required />
              </v-col>
            </v-row>
          </v-container>
        </v-form>
      </v-card-text>
      <v-col xs12 sm12 md12 lg12 xl12>
        <v-card-actions>
          <v-btn color="blue" dark class="mb-2" elevation="4" @click="saveStore" :disabled="!valid">Guardar</v-btn>
          <v-btn color="red" dark class="mb-2" elevation="4" @click="close">Cancelar</v-btn>
        </v-card-actions>
      </v-col>
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
    store: {
      type: Object,
      default: () => ({
        pK_STORE: null,
        storE_NAME: '',
        manager: '',
        address: '',
        phone: '',
        city: '',
        email: ''
      }),
    },
  },
  data() {
    return {
      isOpen: this.modelValue,
      valid: false,
      localStore: { ...this.store },
      toast: useToast(),
      rules: {
        required: (value) => !!value || 'Este campo es requerido.',
      },
    };
  },
  watch: {
    modelValue(newValue) {
      this.isOpen = newValue;
    },
    isOpen(newValue) {
      this.$emit('update:modelValue', newValue);
    },
    store: {
      handler(newStore) {
        this.localStore = { ...newStore };
      },
      deep: true,
    },
  },
  methods: {
    uppercase() {
      this.localStore.storE_NAME = this.localStore.storE_NAME.toUpperCase();
      this.localStore.manager = this.localStore.manager.toUpperCase();
      this.localStore.address = this.localStore.address.toUpperCase();
      this.localStore.city = this.localStore.city.toUpperCase();
      this.localStore.email = this.localStore.email.toUpperCase();
    },
    close() {
      this.isOpen = false;
    },
    async saveStore() {
      if (this.$refs.form.validate()) {
        try {
          if (this.localStore.pK_STORE) {
            await this.$store.dispatch('store/updateStore', { ...this.localStore });
            this.toast.success('Sucursal actualizada con éxito!');
          } else {
            await this.$store.dispatch('store/createStore', { ...this.localStore });
            this.toast.success('Sucursal agregada con éxito!');
          }
          this.$emit('saved', { ...this.localStore });
          this.close();
        } catch (error) {
          if (error.response) {
            this.toast.error('Error en generar la Sucursal.');
          }
        }
      }
    },
  },
};
</script>