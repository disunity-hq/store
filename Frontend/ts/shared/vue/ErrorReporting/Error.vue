<template>
  <div class="error">
    <div class="error-name">{{ name }}</div>
    <div class="error-content">{{ content }}</div>
    <div class="error-copy">
      <button class="btn-primary">
        <i class="fas fa-copy" />
      </button>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import { Component, Prop } from "vue-property-decorator";

@Component
export default class OrgMembersTable extends Vue {
  @Prop({ type: String, required: true }) readonly name: string;
  @Prop({ type: String, required: true }) readonly content: string;
}
</script>

<style lang="scss" scoped>
@import "~@syncfusion/ej2-vue-inplace-editor/styles/bootstrap.scss";
@import "css/variables.scss";

.error {
  display: flex;
  flex-wrap: nowrap;
  align-items: baseline;

  div {
    padding: 1em;
  }

  .error-name {
    font-weight: 600;
    background-color: $red;
    color: $white;
  }

  .error-content {
    flex-grow: 1;
    padding-left: 2em;
    text-align: left;
    background-color: $gray-100;
  }

  .error-copy {
    display: none;
    padding: 0px;
    button {
      padding: 1em 1.5em;
      border: none !important;
    }
  }

  &:hover {
    .error-copy {
      display: block;
      background-color: $blue;
    }
  }
}
</style>
