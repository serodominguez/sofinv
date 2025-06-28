<template>
  <v-dialog v-model="isOpen" max-width="500px" persistent>
    <v-card>
      <v-card-title class="bg-surface-light pt-4">
        <span>{{ localSupplier.pK_SUPPLIER ? 'Editar Proveedor' : 'Agregar Proveedor' }}</span>
      </v-card-title>
      <v-divider></v-divider>
      <v-card-text>
        <v-form ref="form" v-model="valid">
          <v-container>
            <v-row>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localSupplier.companY_NAME"
                  :rules="[rules.required]" counter="50" :maxlength="50" @keyup="uppercase"
                  label="Nombre de la Compañia" required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localSupplier.contact"
                  :rules="[rules.required]" counter="50" :maxlength="50" @keyup="uppercase" label="Contacto" required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localSupplier.phone" counter="8"
                  :maxlength="8" onkeypress="return (event.charCode >= 48 && event.charCode <= 57)"
                  :rules="[rules.required]" label="Teléfono" required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localSupplier.email" counter="50"
                  :maxlength="50" @keyup="uppercase" label="Correo" required />
              </v-col>
            </v-row>
          </v-container>
        </v-form>
      </v-card-text>
      <v-col xs12 sm12 md12 lg12 xl12>
        <v-card-actions>
          <v-btn color="blue" dark class="mb-2" elevation="4" @click="saveSupplier" :disabled="!valid">Guardar</v-btn>
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
    supplier: {
      type: Object,
      default: () => ({
        pK_SUPPLIER: null,
        companY_NAME_NAME: '',
        contact: '',
        phone: '',
        email: ''
      }),
    },
  },
  data() {
    return {
      isOpen: this.modelValue,
      valid: false,
      localSupplier: { ...this.supplier },
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
    supplier: {
      handler(newSupplier) {
        this.localSupplier = { ...newSupplier };
      },
      deep: true,
    },
  },
  methods: {
    uppercase() {
      this.localSupplier.companY_NAME = this.localSupplier.companY_NAME.toUpperCase();
      this.localSupplier.contact = this.localSupplier.contact.toUpperCase();
      this.localSupplier.email = this.localSupplier.email.toUpperCase();
    },
    close() {
      this.isOpen = false;
    },
    async saveSupplier() {
      if (this.$refs.form.validate()) {
        try {
          if (this.localSupplier.pK_SUPPLIER) {
            await this.$store.dispatch('supplier/updateSupplier', { ...this.localSupplier });
            this.toast.success('Proveedor actualizado con éxito!');
          } else {
            await this.$store.dispatch('supplier/createSupplier', { ...this.localSupplier });
            this.toast.success('Proveedor agregado con éxito!');
          }
          this.$emit('saved', { ...this.localSupplier });
          this.close();
        } catch (error) {
          if (error.response) {
            this.toast.error('Error en generar el Proveedor.');
          }
        }
      }
    },
  },
};
</script>