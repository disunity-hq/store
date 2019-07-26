<template>
  <div class="error" :set='message = Message()'>
    <div class="error-name">{{ name }}</div>
    <div class="error-message">{{ message }}</div>
    <div class="error-copy">
      <button class="btn-primary" v-on:click="CopyError(name, context, message)">
        <i class="fas fa-copy" />
      </button>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import { Component, Prop } from "vue-property-decorator";
import copy from "copy-to-clipboard";
import "../syncfusion";

@Component
export default class Error extends Vue {
  @Prop({ type: String, required: true }) readonly name: string;
  @Prop({ type: String, required: false }) readonly context: string;
  @Prop({
    type: Object,
    required: true,
    validator: function(value: Object) {
      if (!value.hasOwnProperty("message")) {
        console.error(
          "Component prop `Error.content` object value missing required `message` property."
        );
        return false;
      }

      return true;
    }
  })
  readonly content: Object;

  public async CopyError(name: string, context: string, message: string) {
    copy(`${name}${context ? "@" + context : ""}: ${message}`);
  }

  public Message(): string {
    return this.content["message"];
  }

  public Tooltip() {
    return "A generic error without any semantics.";
  }
}
</script>

<style lang="scss" scoped>
@import "~@syncfusion/ej2-vue-inplace-editor/styles/bootstrap.scss";
@import "css/variables.scss";

.error {
  height: 4em;
  display: flex;
  flex-wrap: nowrap;
  align-items: stretch;

  > div {
    margin: 0px;
    padding: 0px;
    display: flex;
    align-items: center;
  }

  .error-name {
    font-weight: 600;
    background-color: $red;
    color: $white;
    padding: 0px 1.5em;
  }

  .error-message {
    flex-grow: 1;
    padding-left: 2em;
    text-align: left;
    background-color: $gray-400;
  }

  .error-copy {
    display: none;
    width: 4em;
    .e-tooltip {
      width: 100%;
      height: 100%;
      button {
        width: 100%;
        height: 100%;
        border: none !important;
      }
    }
  }

  &:hover {
    .error-copy {
      display: block;
      background-color: $blue;
    }
  }
}

.tooltipcontainer {
  margin: 0px !important;
  padding: 0px !important;
}
</style>
