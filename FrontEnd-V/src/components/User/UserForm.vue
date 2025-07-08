<template>
  <v-dialog v-model="isOpen" max-width="500px" persistent>
    <v-card>
      <v-card-title class="bg-surface-light pt-4">
        <span>{{ localUser.pK_USER ? 'Editar Usuario' : 'Agregar Usuario' }}</span>
      </v-card-title>
      <v-divider></v-divider>
      <v-card-text>
        <v-form ref="form" v-model="valid">
          <v-container>
            <v-row>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localUser.useR_NAME"
                  :rules="[rules.required]" counter="25" :maxlength="25" @keyup="uppercase" label="Nombre de Usuario"
                  required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localUser.passworD_HASH" type="password"
                  :rules="[rules.required]" counter="15" :maxlength="15" label="Contraseña" clearable required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localUser.names" :rules="[rules.required]"
                  counter="30" :maxlength="30" @keyup="uppercase" label="Nombres" required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localUser.lasT_NAMES"
                  :rules="[rules.required]" counter="30" :maxlength="30" @keyup="uppercase" label="Apellidos"
                  required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localUser.identification" counter="10"
                  :maxlength="10" label="Nº de Carnet" required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localUser.phone"
                  onkeypress="return (event.charCode >= 48 && event.charCode <= 57)" counter="8" :maxlength="8"
                  label="Teléfono" required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-autocomplete color="primary" variant="underlined" :items="roles" v-model="localUser.pK_ROLE"
                  item-title="rolE_NAME" item-value="pK_ROLE" :rules="[rules.required]"
                  no-data-text="No hay datos disponibles" label="Rol" required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-autocomplete color="primary" variant="underlined" :items="stores" v-model="localUser.pK_STORE"
                  item-title="storE_NAME" item-value="pK_STORE" :rules="[rules.required]"
                  no-data-text="No hay datos disponibles" label="Tienda" required />
              </v-col>
            </v-row>
          </v-container>
        </v-form>
        <v-alert v-if="errorMessage" type="error" dismissible>
          {{ errorMessage }}
        </v-alert>
      </v-card-text>
      <v-col xs12 sm12 md12 lg12 xl12>
        <v-card-actions>
          <v-btn color="blue" dark class="mb-2" elevation="4" @click="saveUser" :disabled="!valid">Guardar</v-btn>
          <v-btn color="red" dark class="mb-2" elevation="4" @click="close">Cancelar</v-btn>
        </v-card-actions>
      </v-col>
    </v-card>
  </v-dialog>
</template>

<script>
import { mapGetters } from 'vuex';
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
        useR_NAME: '',
        passworD_HASH: '',
        names: '',
        lasT_NAMES: '',
        identification: '',
        phone: '',
        pK_ROLE: '',
        pK_STORE: ''
      }),
    },
  },
  data() {
    return {
      isOpen: this.modelValue,
      valid: false,
      localUser: { ...this.user },
      oldPassword: '',
      errorMessage: '',
      toast: useToast(),
      rules: {
        required: (value) => !!value || 'Este campo es requerido.',
      },
    };
  },
  computed: {
    ...mapGetters('role', ['roles']),
    ...mapGetters('store', ['stores']),
  },
  watch: {
    modelValue(newValue) {
      this.isOpen = newValue;
    },
    isOpen(newValue) {
      this.$emit('update:modelValue', newValue);
    },
    user: {
      handler(newUser) {
        this.localUser = { ...newUser };
        if (newUser.pK_USER) {
          this.oldPassword = newUser.passworD_HASH;
        } else {
          this.oldPassword = '';
        }
      },
      deep: true,
    },
  },
  mounted() {
    this.$store.dispatch('role/selectRoles');
    this.$store.dispatch('store/selectStores');
  },
  methods: {
    uppercase() {
      this.localUser.useR_NAME = this.localUser.useR_NAME.toUpperCase();
      this.localUser.names = this.localUser.names.toUpperCase();
      this.localUser.lasT_NAMES = this.localUser.lasT_NAMES.toUpperCase();
    },
    close() {
      this.isOpen = false;
    },
    async saveUser() {
      if (this.$refs.form.validate()) {
        try {
          const userData = {
            pK_USER: this.localUser.pK_USER,
            useR_NAME: this.localUser.useR_NAME,
            names: this.localUser.names,
            lasT_NAMES: this.localUser.lasT_NAMES,
            identification: this.localUser.identification,
            phone: this.localUser.phone,
            pK_ROLE: this.localUser.pK_ROLE,
            pK_STORE: this.localUser.pK_STORE,
            updatePassword: false,
          };
          if (this.localUser.pK_USER) {
            if (this.localUser.passworD_HASH !== this.oldPassword) {
              userData.updatePassword = true;
              userData.password = this.localUser.passworD_HASH;
            }
            await this.$store.dispatch('user/updateUser', userData);
            this.toast.success('Usuario actualizado con éxito!');
          } else {
            userData.password = this.localUser.passworD_HASH;
            await this.$store.dispatch('user/createUser', userData);
            this.toast.success('Usuario agregado con éxito!');
          }
          this.$emit('saved', { ...this.localUser });
          this.close();
        } catch (error) {
          if (error.response && error.response.data && error.response.data.message) {
            this.errorMessage = error.response.data.message;
          } else {
            this.errorMessage = 'Error desconocido. Inténtalo de nuevo.';
          }
          if (error.response) {
            this.toast.error('Error en generar el Usuario.');
          }
        }
      }
    },
  },
};
</script>