<template>
  <v-dialog v-model="isOpen" max-width="500px" persistent>
    <v-card>
      <v-card-title class="bg-surface-light pt-4">
        <span>{{ localCustomer.pK_CUSTOMER ? 'Editar Cliente' : 'Agregar Cliente' }}</span>
      </v-card-title>
      <v-divider></v-divider>
      <v-card-text>
        <v-form ref="form" v-model="valid">
          <v-container>
            <v-row>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localCustomer.names" :rules="[rules.required]"
                  counter="30" :maxlength="30" @keyup="uppercase" label="Nombres" required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localCustomer.lasT_NAMES"
                  :rules="[rules.required]" counter="30" :maxlength="30" @keyup="uppercase" label="Apellidos"
                  required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localCustomer.identification" counter="10"
                  :maxlength="10" label="Nº de Carnet" required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localCustomer.phone"
                  onkeypress="return (event.charCode >= 48 && event.charCode <= 57)" counter="8" :maxlength="8"
                  label="Teléfono" required />
              </v-col>
            </v-row>
          </v-container>
        </v-form>
      </v-card-text>
      <v-col>
        <v-card-actions>
          <v-btn color="blue" dark class="mb-2" elevation="4" @click="saveCustomer" :disabled="!valid">Guardar</v-btn>
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
    customer: {
      type: Object,
      default: () => ({
        pK_CUSTOMER: null,
        names: '',
        lasT_NAMES: '',
        identification: '',
        phone: ''
      }),
    },
  },
  data() {
    return {
      isOpen: this.modelValue,
      valid: false,
      localCustomer: { ...this.customer },
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
    customer: {
      handler(newCustomer) {
        this.localCustomer = { ...newCustomer };
      },
      deep: true,
    },
  },
  methods: {
    uppercase() {
      this.localCustomer.names = this.localCustomer.names.toUpperCase();
      this.localCustomer.lasT_NAMES = this.localCustomer.lasT_NAMES.toUpperCase();
    },
    close() {
      this.isOpen = false;
    },
    async saveCustomer() {
      if (this.$refs.form.validate()) {
        try {
          if (this.localCustomer.pK_CUSTOMER) {
            await this.$store.dispatch('customer/updateCustomer', { ...this.localCustomer });
            this.toast.success('Cliente actualizado con éxito!');
          } else {
            await this.$store.dispatch('customer/createCustomer', { ...this.localCustomer });
            this.toast.success('Cliente agregado con éxito!');
          }
          this.$emit('saved', { ...this.localCustomer });
          this.close();
        } catch (error) {
          if (error.response) {
            this.toast.error('Error en generar el Cliente.');
          }
        }
      }
    },
  },
};
</script>